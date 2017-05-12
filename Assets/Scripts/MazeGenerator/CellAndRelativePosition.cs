public class CellAndRelativePosition
{

    public Cell cell;
    public Direction direction; // next wall to be deleted

    public enum Direction
    {
        North,
        South,
        East,
        West
    }


    public CellAndRelativePosition(Cell cell, Direction direction)
    {
        this.cell = cell;
        this.direction = direction;
    }

}
