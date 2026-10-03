namespace AutoVision.Service.IPC.Factory
{
    /// <summary>Creates a concrete <see cref="IIpc"/> for one hardware maker (WMX, SIM, …).</summary>
    public interface IIpcMakerFactory
    {
        MAKER Maker { get; }
        IIpc Create();
    }
}
