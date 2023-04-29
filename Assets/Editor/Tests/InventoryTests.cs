
using LD52;
using NUnit.Framework;
using UnityEngine;

public class InventoryTests
{
    [Test]
    public void InventoryPrintTest()
    {
        var inventory = new Inventory();
        inventory.Height = 2;
        inventory.Width = 5;
        Debug.Log( inventory.PrintInventory());
    }

    [Test]
    public void PutOneCellItemToInventoryTest()
    {
        var inventory = new Inventory();
        inventory.Height = 2;
        inventory.Width = 5;

        var success = inventory.TryAddItem(new ItemDescription()
        {
            Size = new Vector2Int(1, 1),
            Id = "SingleItem"
        });
        
        Assert.IsTrue(success);
        Assert.AreEqual(0, inventory.Taken[0,0]);
        Debug.Log( inventory.PrintInventory());
    }
    
    [Test]
    public void Put2x2CellItemToInventoryTest()
    {
        var inventory = new Inventory();
        inventory.Height = 5;
        inventory.Width = 2;

        var success = inventory.TryAddItem(new ItemDescription()
        {
            Size = new Vector2Int(2, 2),
            Id = "SingleItem"
        });
        
        Assert.IsTrue(success);
        Assert.AreEqual(0, inventory.Taken[0,0]);
        Assert.AreEqual(0, inventory.Taken[1,0]);
        Assert.AreEqual(0, inventory.Taken[1,1]);
        Assert.AreEqual(0, inventory.Taken[0,1]);
        Debug.Log( inventory.PrintInventory());
    }
    
    [Test]
    public void PutItem3ToInventoryTest()
    {
        var inventory = new Inventory();
        inventory.Height = 5;
        inventory.Width = 2;

        var success = inventory.TryAddItem(new ItemDescription()
        {
            Size = new Vector2Int(2, 2),
            Id = "SingleItem"
        });
        
        var success2 = inventory.TryAddItem(new ItemDescription()
        {
            Size = new Vector2Int(2, 2),
            Id = "SingleItem"
        });
        
        Assert.IsTrue(success);
        Assert.IsTrue(success2);
        Assert.AreEqual(0, inventory.Taken[0,0]);
        Assert.AreEqual(0, inventory.Taken[1,0]);
        Assert.AreEqual(0, inventory.Taken[1,1]);
        Assert.AreEqual(0, inventory.Taken[0,1]);
        
        Assert.AreEqual(1, inventory.Taken[2,0]);
        Assert.AreEqual(1, inventory.Taken[3,0]);
        Assert.AreEqual(1, inventory.Taken[3,1]);
        Assert.AreEqual(1, inventory.Taken[2,1]);
        Debug.Log( inventory.PrintInventory());
    }
    
    [Test]
    public void PutTooBigItem()
    {
        var inventory = new Inventory();
        inventory.Height = 5;
        inventory.Width = 2;

        var success = inventory.TryAddItem(new ItemDescription()
        {
            Size = new Vector2Int(3, 3),
            Id = "SingleItem"
        });
        
        Assert.IsFalse(success);
        Debug.Log( inventory.PrintInventory());
    }

    
}
