using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuildingSystem3D : MonoBehaviour {

    public static GridBuildingSystem3D Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;


    private GridXZ<GridObject> grid;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
    [SerializeField] private GameObject parentObject;
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir;

    private void Awake() {
        Instance = this;

        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));

        placedObjectTypeSO = null;// placedObjectTypeSOList[0];
    }

    public class GridObject {

        private GridXZ<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject_Done placedObject;

        public GridObject(GridXZ<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject_Done placedObject) {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearPlacedObject() {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject_Done GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return placedObject == null;
        }

    }

    private void Update() {     
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null && !MouseCheckerManager.instance.mouseCheck) {
            bool isRoad = placedObjectTypeSO.isRoad;
            bool isStartFinish = placedObjectTypeSO.isStartFinish;
            bool isTurn = placedObjectTypeSO.isTurn;
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    canBuild = false;
                    break;
                }
            }

            bool moneyCheck = false;
            if (GameManager.instance.money.EnoughMoney(placedObjectTypeSO.cost))
            {
                moneyCheck = true;
            }

            bool isStartFinishPlaced = true;
            bool canBuildRoad = true;
            bool trueDir = true;
            if(isRoad)
            {
                if(RaceSystem.instance.startFinish == Vector3.zero && !isStartFinish)
                {
                    isStartFinishPlaced = false;
                }
                if(Vector3.Distance(grid.GetWorldPosition(x,z) + new Vector3(5,0,5), RaceSystem.instance.roads.LastOrDefault()) > 10 && RaceSystem.instance.roads.Any())
                {
                    canBuildRoad = false;
                }
                else if(Vector3.Distance(grid.GetWorldPosition(x,z) + new Vector3(5,0,5), RaceSystem.instance.startFinish) > 10 && !RaceSystem.instance.roads.Any())
                {
                    canBuildRoad = false;
                }        
                if(isStartFinish)
                {
                    canBuildRoad = true;
                    if(RaceSystem.instance.startFinish != Vector3.zero)
                    {
                        isStartFinishPlaced = false;
                    }
                }
                if(canBuild && isStartFinishPlaced && canBuildRoad && moneyCheck)
                {
                    trueDir = DirectionCheck(grid.GetWorldPosition(x, z), isTurn, isStartFinish, dir);
                }
            }

            if (moneyCheck) 
            {
                if(isStartFinishPlaced && canBuildRoad && trueDir)
                {
                    if (canBuild)
                    {
                        Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                        Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                        UtilsClass.CreateWorldTextPopup("Builded!", mousePosition);
                        PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
                        placedObject.transform.SetParent(parentObject.transform);
                        
                        foreach (Vector2Int gridPosition in gridPositionList) 
                        {
                            grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                        }

                        if(isRoad)
                        {
                            if(isStartFinish)
                            {
                                RaceSystem.instance.startFinish = grid.GetWorldPosition(x, z) + new Vector3(5,0,5);
                                RaceSystem.instance.startFinishDir = dir;
                            }
                            else
                            {
                                RaceSystem.instance.roads.Add(grid.GetWorldPosition(x, z) + new Vector3(5,0,5));
                            }
                        }
                        
                        OnObjectPlaced?.Invoke(this, EventArgs.Empty);

                        GameManager.instance.money.UseMoney(placedObjectTypeSO.cost);

                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            DeselectObjectType();
                        }
                    }
                    else
                    {
                        UtilsClass.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
                    }
                }
                else if(!isStartFinishPlaced)
                {
                    if(RaceSystem.instance.startFinish == Vector3.zero)
                    {
                        UtilsClass.CreateWorldTextPopup("Build Start Finish First", mousePosition);
                    }
                    else
                    {
                        UtilsClass.CreateWorldTextPopup("Start Finish Is Alredy Placed", mousePosition);
                    }
                }
                else if(!canBuildRoad)
                {
                    UtilsClass.CreateWorldTextPopup("No Nearby Roads Found", mousePosition);
                }
                else if(!trueDir)
                {
                    UtilsClass.CreateWorldTextPopup("Wrong Direction", mousePosition);
                }
            } 
            else 
            {
                UtilsClass.CreateWorldTextPopup("Not Enough Money!", mousePosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { DeselectObjectType(); }


        if (Input.GetMouseButtonDown(1)) {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();

            if (grid.GetGridObject(mousePosition) != null) {
                PlacedObject_Done placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
                grid.GetXZ(mousePosition, out int x, out int z);
                Vector3 position = grid.GetWorldPosition(x, z) + new Vector3(5,0,5);
                bool demolishPossible = true;
                if(RaceSystem.instance.roads.Contains(position) && position != RaceSystem.instance.roads.LastOrDefault())
                {
                    UtilsClass.CreateWorldTextPopup("Demolish impossible", mousePosition);
                    demolishPossible = false;
                }
                if (placedObject != null && demolishPossible) {
                    if(RaceSystem.instance.roads.Contains(position) && RaceSystem.instance.roads.Any())
                    {
                        RaceSystem.instance.roads.RemoveAt(RaceSystem.instance.roads.Count - 1);
                    }
                    if(position == RaceSystem.instance.startFinish)
                    {
                        RaceSystem.instance.startFinish = Vector3.zero;
                    }
                    placedObject.DestroySelf();
                    UtilsClass.CreateWorldTextPopup("Demolished", mousePosition);

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList) {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }
        }
    }

    private void DeselectObjectType() {
        placedObjectTypeSO = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (placedObjectTypeSO != null) {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

    public void GetObjectID(int ID){
        placedObjectTypeSO = placedObjectTypeSOList[ID]; RefreshSelectedObjectType();
    }

    private bool DirectionCheck(Vector3 worldPosition, bool isTurn, bool isStartFinish, PlacedObjectTypeSO.Dir dir)
    {
        float x1 = (worldPosition + new Vector3(5,0,5)).x;
        float z1 = (worldPosition + new Vector3(5,0,5)).z;
        bool trueDir = true;
        if(!RaceSystem.instance.roads.Any())
        {
            float x2 = RaceSystem.instance.startFinish.x;
            float z2 = RaceSystem.instance.startFinish.z;
            switch(RaceSystem.instance.startFinishDir)
            {
                default:
                case PlacedObjectTypeSO.Dir.Left:
                case PlacedObjectTypeSO.Dir.Right:
                    if(x1 > x2 && z1 == z2)
                    {
                        if(isTurn)
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Left)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Down;
                                break;
                            }
                            else if(dir == PlacedObjectTypeSO.Dir.Up)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Up;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                        else
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Left || dir == PlacedObjectTypeSO.Dir.Right)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Right;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                    }
                    else if(z1 == z2)
                    {
                        if(isTurn)
                        {   
                            if(dir == PlacedObjectTypeSO.Dir.Right)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Up;
                                break;
                            }
                            else if(dir == PlacedObjectTypeSO.Dir.Down)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Down;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                        else
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Left || dir == PlacedObjectTypeSO.Dir.Right)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Left;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        trueDir = false;
                        break;
                    }
                case PlacedObjectTypeSO.Dir.Up:
                case PlacedObjectTypeSO.Dir.Down:
                    if(z1 > x2 && x1 == x2)
                    {
                        if(isTurn)
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Left)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Left;
                                break;
                            }
                            else if(dir == PlacedObjectTypeSO.Dir.Down)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Right;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                        else
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Up || dir == PlacedObjectTypeSO.Dir.Down)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Up;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                    }
                    else if(x1 == x2)
                    {
                        if(isTurn)
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Right)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Right;
                                break;
                            }
                            else if(dir == PlacedObjectTypeSO.Dir.Up)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Left;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                        else
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Up || dir == PlacedObjectTypeSO.Dir.Down)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Down;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        trueDir = false;
                        break;
                    }
            }
        }
        else
        {
            float x2 = RaceSystem.instance.roads.LastOrDefault().x;
            float z2 = RaceSystem.instance.roads.LastOrDefault().z;
            switch(RaceSystem.instance.trackDir)
            {
                default:
                case PlacedObjectTypeSO.Dir.Up:
                    if(x1 == x2)
                    {
                        if(isTurn)
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Left)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Left;
                                break;
                            }
                            else if(dir == PlacedObjectTypeSO.Dir.Down)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Right;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                        else
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Up || dir == PlacedObjectTypeSO.Dir.Down)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Up;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        trueDir = false;
                        break;
                    }
                case PlacedObjectTypeSO.Dir.Down:
                    if(x1 == x2)
                    {
                        if(isTurn)
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Up)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Left;
                                break;
                            }
                            else if(dir == PlacedObjectTypeSO.Dir.Right)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Right;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                        else
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Up || dir == PlacedObjectTypeSO.Dir.Down)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Down;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        trueDir = false;
                        break;
                    }
                case PlacedObjectTypeSO.Dir.Left:
                    if(z1 == z2)
                    {
                        if(isTurn)
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Right)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Up;
                                break;
                            }
                            else if(dir == PlacedObjectTypeSO.Dir.Down)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Down;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                        else
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Left || dir == PlacedObjectTypeSO.Dir.Right)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Left;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        trueDir = false;
                        break;
                    }
                case PlacedObjectTypeSO.Dir.Right:
                    if(z1 == z2)
                    {
                        if(isTurn)
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Up)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Up;
                                break;
                            }
                            else if(dir == PlacedObjectTypeSO.Dir.Left)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Down;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                        else
                        {
                            if(dir == PlacedObjectTypeSO.Dir.Left || dir == PlacedObjectTypeSO.Dir.Right)
                            {
                                trueDir = true;
                                RaceSystem.instance.trackDir = PlacedObjectTypeSO.Dir.Right;
                                break;
                            }
                            else
                            {
                                trueDir = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        trueDir = false;
                        break;
                    }
            }
        }
        if(isStartFinish)
        {
            trueDir = true;
        }
        return trueDir;
    }
}
