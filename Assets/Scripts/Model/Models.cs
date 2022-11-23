using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 全局数据空间类
/// </summary>
public class Models
{
    /// <summary>
    /// 游戏数据
    /// </summary>
    public static GameModel GameModel { get; set; }

    static Models()
    {
        GameModel = new GameModel();
    }
}