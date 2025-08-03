mergeInto(LibraryManager.library, {
    IsFullscreen: function () {
        return document.fullscreenElement !== null;
    }
});