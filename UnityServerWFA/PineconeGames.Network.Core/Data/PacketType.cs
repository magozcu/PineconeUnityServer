namespace PineconeGames.Network.Core.Data
{
    #region Event Handlers

    public delegate void OnRingEventHandler();

    #endregion

    public enum PacketType
    {
        Undefined,
        Welcome,
        WelcomeReceived,
    }
}