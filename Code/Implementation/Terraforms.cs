namespace Cultivation_Way.Implementation;

internal static class Terraforms
{
    public static void init()
    {
        TerraformOptions terraform = null;
        terraform = AssetManager.terraform.clone("cw_fall_mountain", "meteorite");
        terraform.damage = 0;
        terraform.shake = false;
        terraform.explode_and_set_random_fire = false;
        terraform.applyForce = false;

        AssetManager.terraform.add(terraform);
    }
}