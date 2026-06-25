using Xunit;
using Moq;
using task04;

namespace task04test;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();

        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();

        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();

        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeWeakerThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();

        Assert.True(fighter.FirePower < cruiser.FirePower);
    }

    [Fact]
    public void Cruiser_MoveForwardTest()
    {
        var cruiser = new Cruiser();

        cruiser.MoveForward();
        Assert.Equal(50, cruiser.Distance);

        cruiser.MoveForward();
        Assert.Equal(100, cruiser.Distance);
    }

    [Fact]
    public void Cruiser_RotateTest()
    {
        var cruiser = new Cruiser();

        cruiser.Rotate(15);
        Assert.Equal(15, cruiser.Angle);

        cruiser.Rotate(345);
        Assert.Equal(0, cruiser.Angle);
    }

    [Fact]
    public void Cruiser_FireTest()
    {
        var cruiser = new Cruiser();

        cruiser.Fire();
        Assert.Equal(44, cruiser.Bullets);

        for(int i = 0; i < 44; i++) cruiser.Fire();
        Assert.Equal(0, cruiser.Bullets);

        cruiser.Fire();
        Assert.Equal(0, cruiser.Bullets);
    }

    [Fact]
    public void Fighter_MoveForwardTest()
    {
        var fighter = new Fighter();

        fighter.MoveForward();
        Assert.Equal(100, fighter.Distance);

        fighter.MoveForward();
        Assert.Equal(200, fighter.Distance);
    }

    [Fact]
    public void Fighter_RotateTest()
    {
        var fighter = new Fighter();

        fighter.Rotate(120);
        Assert.Equal(120, fighter.Angle);

        fighter.Rotate(260);
        Assert.Equal(20, fighter.Angle);
    }

    [Fact]
    public void Fighter_FireTest()
    {
        var fighter = new Fighter();

        fighter.Fire();
        Assert.Equal(24, fighter.Bullets);

        for(int i = 0; i < 24; i++) fighter.Fire();
        Assert.Equal(0, fighter.Bullets);

        fighter.Fire();
        Assert.Equal(0, fighter.Bullets);
    }
}
