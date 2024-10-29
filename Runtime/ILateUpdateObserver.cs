public interface ILateUpdateObserver
{
    void ObservedLateUpdate();


    void OnEnable()
    {
        UpdatePublisher.RegisterLateUpdateObserver(this);
    }


    void OnDisable()
    {
        UpdatePublisher.UnregisterLateUpdateObserver(this);
    }
}