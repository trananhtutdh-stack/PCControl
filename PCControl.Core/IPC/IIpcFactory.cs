namespace AutoVision.Service.IPC
{
    public interface IIpcFactory
    {
        IIpc Create(MAKER maker);
    }
}
