/** 高开销的灵气更新
                    chunk_1 = chunks[i, j]; chunk_2 = chunks[i, j + 1];
                    delta_wakan = chunk_2.wakan - chunk_1.wakan;
                    delta_wakan_level = chunk_2.wakan_level - chunk_1.wakan_level;

                    delta_wakan_1 = delta_wakan * delta_wakan_level * Others.CW_Constants.chunk_wakan_spread_grad * Mathf.Pow(Others.CW_Constants.wakan_level_co, delta_wakan_level * Others.CW_Constants.chunk_wakan_level_spread_grad - chunk_1.wakan_level);

                    delta_wakan_2 = delta_wakan * delta_wakan_level * Others.CW_Constants.chunk_wakan_spread_grad;

                    delta_wakan_level_2 = delta_wakan_level * Others.CW_Constants.chunk_wakan_level_spread_grad;

                    delta_wakan_level_1 = Mathf.Log(
                        (chunk_1.wakan * Mathf.Pow(Others.CW_Constants.wakan_level_co, chunk_1.wakan_level)+chunk_2.wakan * Mathf.Pow(Others.CW_Constants.wakan_level_co, chunk_2.wakan_level)-(chunk_2.wakan+delta_wakan_2)*Mathf.Pow(Others.CW_Constants.wakan_level_co, chunk_2.wakan_level + delta_wakan_level_2))/(chunk_1.wakan + delta_wakan_1) , 
                        Others.CW_Constants.wakan_level_co);

                    chunk_1.tmp_wakan += delta_wakan_1;
                    chunk_2.tmp_wakan += delta_wakan_2;
                    chunk_1.total_wakan += delta_wakan_level_1;
                    chunk_2.total_wakan += delta_wakan_level_2;
*/
/** 建筑周围地块检查
internal bool checkTilesForUpgrade(WorldTile pTile, BuildingAsset pTemplate)
{
	int num = pTile.pos.x - pTemplate.fundament.left;
	int num2 = pTile.pos.y - pTemplate.fundament.bottom;
	int num3 = pTemplate.fundament.right + pTemplate.fundament.left + 1;
	int num4 = pTemplate.fundament.top + pTemplate.fundament.bottom + 1;
	for (int i = 0; i < num3; i++)
	{
		for (int j = 0; j < num4; j++)
		{
			WorldTile tile = this.world.GetTile(num + i, num2 + j);
			if (tile == null)
			{
				MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of tile==null", this.stats.id, pTile.x, pTile.y));
				return false;
			}
			if (!tile.Type.canBuildOn)
			{
				MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of !tile.Type.canBuildOn", this.stats.id, pTile.x, pTile.y));
				return false;
			}
			if (tile.zone.city != this.city)
			{
				MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of tile.zone.city != this.city", this.stats.id, pTile.x, pTile.y));
				return false;
			}
			Building building = tile.building;
			if (building != null && building != this)
			{
				if (building.stats.priority >= this.stats.priority)
				{
					MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of lower priority than '{3}', {4}<={5}", new object[]
					{
						this.stats.id,
						pTile.x,
						pTile.y,
						building.stats.id,
						this.stats.priority,
						building.stats.priority
					}));
					return false;
				}
				if (building.stats.upgradeLevel >= this.stats.upgradeLevel)
				{
					MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of lower upgradeLevel than '{3}', {4}<={5}", new object[]
					{
						this.stats.id,
						pTile.x,
						pTile.y,
						building.stats.id,
						this.stats.upgradeLevel,
						building.stats.upgradeLevel
					}));
					return false;
				}
			}
		}
	}
	return true;
}
*/

