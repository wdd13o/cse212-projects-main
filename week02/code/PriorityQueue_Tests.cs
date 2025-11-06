using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Test that items are dequeued in priority order
    // Expected Result: Items should be dequeued in order of highest to lowest priority
    // Defect(s) Found: Dequeue does not check full list for highest priority
    //                  Does not remove item after dequeuing
    public void TestPriorityQueue_HighestPriorityFirst()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("low", 1);
        priorityQueue.Enqueue("medium", 5);
        priorityQueue.Enqueue("high", 10);

        Assert.AreEqual("high", priorityQueue.Dequeue());
        Assert.AreEqual("medium", priorityQueue.Dequeue());
        Assert.AreEqual("low", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Test FIFO behavior for items with equal priority
    // Expected Result: Items with equal priority should be dequeued in the order they were enqueued
    // Defect(s) Found: Logic uses >= in priority comparison which violates FIFO for equal priorities
    public void TestPriorityQueue_FIFOForEqualPriority()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("first", 5);
        priorityQueue.Enqueue("second", 5);
        priorityQueue.Enqueue("third", 5);

        Assert.AreEqual("first", priorityQueue.Dequeue());
        Assert.AreEqual("second", priorityQueue.Dequeue());
        Assert.AreEqual("third", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Test empty queue behavior
    // Expected Result: Should throw InvalidOperationException with message "The queue is empty."
    // Defect(s) Found: None - empty queue handling works correctly
    public void TestPriorityQueue_EmptyQueue()
    {
        var priorityQueue = new PriorityQueue();
        
        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Should have thrown an exception");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual("The queue is empty.", ex.Message);
        }
    }

    [TestMethod]
    // Scenario: Test mixed priorities with multiple dequeues
    // Expected Result: Items should be dequeued in strict priority order, with FIFO for equal priorities
    // Defect(s) Found: Items not removed after dequeue, affecting subsequent dequeues
    public void TestPriorityQueue_MixedPriorities()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("first-10", 10);
        priorityQueue.Enqueue("second-10", 10);
        priorityQueue.Enqueue("first-5", 5);
        priorityQueue.Enqueue("first-1", 1);
        priorityQueue.Enqueue("second-5", 5);

        Assert.AreEqual("first-10", priorityQueue.Dequeue());
        Assert.AreEqual("second-10", priorityQueue.Dequeue());
        Assert.AreEqual("first-5", priorityQueue.Dequeue());
        Assert.AreEqual("second-5", priorityQueue.Dequeue());
        Assert.AreEqual("first-1", priorityQueue.Dequeue());
    }
}