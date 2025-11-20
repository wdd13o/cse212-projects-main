public class Node
{
    public int Data { get; set; }
    public Node? Right { get; private set; }
    public Node? Left { get; private set; }

    public Node(int data)
    {
        this.Data = data;
    }

    public void Insert(int value)
    {
        // Only insert unique values (do not insert duplicates)
        if (value < Data)
        {
            // Insert to the left
            if (Left is null)
                Left = new Node(value);
            else
                Left.Insert(value);
        }
        else if (value > Data)
        {
            // Insert to the right
            if (Right is null)
                Right = new Node(value);
            else
                Right.Insert(value);
        }
        // if value == Data, do nothing (no duplicates allowed)
    }

    public bool Contains(int value)
    {
        if (value == Data)
            return true;

        if (value < Data)
            return Left is not null && Left.Contains(value);
        else
            return Right is not null && Right.Contains(value);
    }

    public int GetHeight()
    {
        int leftHeight = Left is not null ? Left.GetHeight() : 0;
        int rightHeight = Right is not null ? Right.GetHeight() : 0;
        return 1 + (leftHeight > rightHeight ? leftHeight : rightHeight);
    }
}