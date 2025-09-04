using UnityEngine;
/// <summary>
/// ��Ƭ��ק�¼������࣬��չ֧��Ŀ������Ϣ
/// </summary>
public class CardDragEventArgs : System.EventArgs
{
    /// <summary>
    /// �����¼��Ŀ�Ƭʵ��
    /// </summary>
    public DraggableCard Card { get; }

    /// <summary>
    /// �¼�����ʱ������Ļ���꣨���أ�
    /// </summary>
    public Vector3 MousePosition { get; }

    /// <summary>
    /// �¼�����ʱ��Ƭ����������λ��
    /// </summary>
    public Vector3 CardPosition { get; }

    /// <summary>
    /// ��Ƭ���õ�Ŀ���ۣ���ѡ�����ڷ����¼�����Ч��
    /// </summary>
    public CardSlot TargetSlot { get; set; }

    /// <summary>
    /// ���캯������ʼ���¼�����
    /// </summary>
    /// <param name="card">�����¼��Ŀ�Ƭ</param>
    /// <param name="mousePos">�����Ļ����</param>
    /// <param name="cardPos">��Ƭ��������</param>
    public CardDragEventArgs(DraggableCard card, Vector3 mousePos, Vector3 cardPos)
    {
        Card = card;
        MousePosition = mousePos;
        CardPosition = cardPos;
        TargetSlot = null; // Ĭ��Ϊ�գ���Ҫʱ���¼�����ǰ����
    }
}