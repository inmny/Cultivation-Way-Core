using System.Collections.Generic;
using System.IO;
using System.Text;
using Cultivation_Way.Constants;
using NeoModLoader.api.attributes;
namespace Cultivation_Way.Test;

internal class DamageRecord
{
    public float Armor;
    public float ArmorReduce;
    public int AttackerLevel;
    public CW_AttackType AttackType;
    public float Damage;
    public int DefenderLevel;
    public float LevelReduce;
    public float RawDamage;
    public float TotalReduce;
    public DamageRecord(CW_AttackType pAttackType, float pRawDamage, int pAttackerLevel, int pDefenderLevel, float pArmor, float pArmorReduce, float pLevelReduce)
    {
        AttackType = pAttackType;
        RawDamage = pRawDamage;
        AttackerLevel = pAttackerLevel;
        DefenderLevel = pDefenderLevel;
        Armor = pArmor;
        ArmorReduce = pArmorReduce;
        LevelReduce = pLevelReduce;

        TotalReduce = pArmorReduce * pLevelReduce;
        Damage = RawDamage * TotalReduce;
        if (Damage < 1) Damage = 0;
    }

    public override string ToString()
    {
        return $"{AttackType},{(int)RawDamage},{(int)Damage},{AttackerLevel},{DefenderLevel},{(int)Armor},{(long)(ArmorReduce * 1000)},{(long)(LevelReduce * 1000)},{(long)(TotalReduce * 1000000)}";
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj is not DamageRecord record) return false;
        return record.AttackType == AttackType &&
               (int)record.RawDamage == (int)RawDamage && (int)record.Damage == (int)Damage &&
               record.AttackerLevel == AttackerLevel && record.DefenderLevel == DefenderLevel &&
               (int)record.Armor == (int)Armor &&
               (long)(record.ArmorReduce * 1000) == (int)(1000 * ArmorReduce) &&
               (long)(1000 * record.LevelReduce) == (long)(1000 * LevelReduce) &&
               (long)(1000000 * record.TotalReduce) == (long)(1000000 * TotalReduce);
    }
}

internal static class DamageRecordManager
{
    private static readonly HashSet<DamageRecord> records = new();
    public static void AddDamageRecord(DamageRecord record)
    {
        records.Add(record);
    }
    public static void Clear()
    {
        records.Clear();
    }

    public static void AddDamageRecord(CW_AttackType pAttackType, float pRawDamage, int pAttackerLevel,
        int pDefenderLevel, float pArmor, float pArmorReduce, float pLevelReduce)
    {
        if (!CW_Core.mod_state.editor_inmny) return;
        AddDamageRecord(new DamageRecord(pAttackType, pRawDamage, pAttackerLevel, pDefenderLevel, pArmor, pArmorReduce, pLevelReduce));
    }
    [Hotfixable]
    public static void Save()
    {
        if (records.Count < 100) return;
        string save_path = Path.Combine(CW_Core.Instance.GetDeclaration().FolderPath,
            $".DamageRecord-{World.world.mapStats.getCurrentYear()}-{World.world.mapStats.getCurrentMonth()}.csv");

        StringBuilder sb = new();
        sb.AppendLine("AttackType,RawDamage,Damage,AttackerLevel,DefenderLevel,Armor,ArmorReduce,LevelReduce,TotalReduce");
        foreach (var record in records)
        {
            sb.AppendLine($"{record.AttackType},{record.RawDamage},{record.Damage},{record.AttackerLevel},{record.DefenderLevel},{record.Armor},{record.ArmorReduce},{record.LevelReduce},{record.TotalReduce}");
        }
        File.WriteAllText(save_path, sb.ToString());
    }
}