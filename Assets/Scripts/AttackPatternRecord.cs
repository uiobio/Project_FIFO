public class AttackPatternRecord
{
    public const int MaxPatternLen = 3;
    private int recordTopIndex;
    private AttackType[] record;

    public AttackPatternRecord()
    {
        recordTopIndex = 0;
        record = new AttackType[MaxPatternLen];
    }

    public void AddAttack(AttackType attackType)
    {
        if (recordTopIndex > record.Length)
        {
            throw new System.Exception();
        }
    }
}