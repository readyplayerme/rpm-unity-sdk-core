namespace ReadyPlayerMe.Core
{
    public enum FailureType
    {
        None,
        NoInternetConnection,
        UrlProcessError,
        ShortCodeError,
        DownloadError,
        MetadataDownloadError,
        MetadataParseError,
        ModelDownloadError,
        ModelImportError,
        DirectoryAccessError,
        AvatarProcessError,
        AvatarRenderError,
        OperationCancelled
    }
}
