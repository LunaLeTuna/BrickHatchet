using System;
using UnityEngine;
using Utils;

[Serializable]
public class Brick
{
    // obj properties
    public string Name; // Brick Name
    public Vector3 Position; // XYZ Position
    public Vector3 Scale; // XYZ Scale
    public Vector3 Rotation; // XYZ Rotation
    public Color BrickColor; // Color of the brick.
    public float Transparency = 1f; // Transparency of the brick. This might be able to be replaced with BrickColor.a
    public bool CollisionEnabled = true; // Can you collide with the brick?
    public string Model; // ID of the asset used for the brick
    public KEType KE_Type; // this determins if it's a prop or maybe a light... could be anything
    public string missing_type; // if there is no KE_Type when importing we just throw it here for export

    // bh properties
    public bool is_bh = false;
    public bool Clickable = false; // Is the brick clickable?
    public float ClickDistance; // Distance brick can be clicked from
    public ShapeType Shape = ShapeType.cube; // Brick shape

    // bb properties
    public bool ScuffedScale = false;
    public bool Selected = false;
    public int ID; // ID of the brick, makes many things much easier
    public BrickGroup Parent; // parent
    public GameObject gameObject; // the GameObject built from this brick
    public BrickGO brickGO; // BrickGO of the above gameobject
    public BrickShape brickShape; // BrickShape of the above gameobject, manages brick scaling and stuff

    // Convert from BH Transform Values to Unity - Use this when reading bricks from file
    public void ConvertTransformToUnity () {
        Vector3 pos = Position.SwapYZ() + Scale.SwapYZ() / 2; // Brick-Hill positions are based around some corner of the brick, while Unity positions are based around the pivot point (usually center) of the transform
        pos.x *= -1; // Flip x axis
        Position = pos;

        Scale = Scale.SwapYZ();
        if (Rotation.y != 0 && Rotation.y != 180) Scale = Scale.SwapXZ();

        Rotation.y = Rotation.y * -1; // Invert rotation
        Rotation.y = Rotation.y%360; // keep rotation between 0-359


        //Utils.Math.Mod(Mathf.RoundToInt(target.eulerAngles.y), 360);
    }

    // Convert from Unity Transform Values to BH and return brickdata (does not overwrite brick) - use this when exporting bricks
    public BrickData ConvertTransformToBH () {
        return new BrickData(); // todo
    }

    // call this when scale is changed
    public void UpdateShape () {
        brickShape.UpdateShape();
    }

    // call when model is changed
    public void UpdateModel () {
        if (string.IsNullOrWhiteSpace(Model)) {
            brickShape.RemoveAssetGameobject();
        } else {
            CustomModelHelper.SetCustomModel(brickShape, Model);
        }
    }

    // call this when rotation is changed
    public void CheckIfScuffed () {
        if (!((Rotation.y == 0 || Rotation.y == 180) ^ ScuffedScale)) { // massive brain code that i did not figure out myself
            Scale = Scale.SwapXY();
            Position = gameObject.transform.position.ToBB(Scale);
            ScuffedScale = !ScuffedScale;
        }
    }

    // also call this when scale is changed?
    public void ClampSize () {
        if (MapBuilder.instance.ShapeConstraints.TryGetValue(Shape, out ShapeSizeConstraint ssc)) {
            Scale = Scale.Clamp(ssc.min, ssc.max);
        }
    }

    public enum KEType {
        Legacy_Brick,
        Prop,
        Light,
        Spawn_Point,
        Obsolete
    }

    // plate, corner, corner_inv, and round are not included because:
    // plate is literally just a cube with the height set to 0.3, so why does it need to be separate?
    // corner, corner_inv, and round may be added at some point, but for now no, as they are broken in bh and i don't believe they are used often
    public enum ShapeType {
        cube,
        slope,
        wedge,
        spawnpoint,
        arch,
        dome,
        bars,
        flag,
        pole,
        cylinder,
        round_slope,
        vent
    }
}
