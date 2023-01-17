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

