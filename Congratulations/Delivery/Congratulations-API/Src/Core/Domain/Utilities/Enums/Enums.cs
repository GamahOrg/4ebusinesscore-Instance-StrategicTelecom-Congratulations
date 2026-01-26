namespace _4EPlatform.Domain.Utilities.Enums
{
    public enum State
    {
        NotDeleted,
        Deleted
    }
    public enum StatusEnum
    {
        FailedToSave,
        SavedSuccessfully,
        Exception,
        Success,
        Failed,
        FailedToFindTheObject,
        AlreadyExisting
    }
    public enum ActionNameEnum
    {
        Get,
        Post,
        Put,
        Delete,
    }

    public enum EventsStatus
    {
        Publish,
        Archieve
    }

    public enum OrderStatus
    {
        Latest,
        Oldest
    }
    public enum CongratStatus
    {
        Permanent,
        Temporary,
        Stopped
    }
}