// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
let connection = null;
let sigEvents = {
    OnStateUpdated: (e) => {},
    OnNewMessage: (e) => {},
    OnConnected: () => {},
    OnError: (err) => console.error(err.toString())
};

let telegramSessionState = [
    "Uninitialized",
    "Initializing",
    "Initialized",
    "SettingEncryptionKey",
    "Connected",
    "InError",
    "Unauthorized",
    "PendingServiceAccount",
    "PendingCode",
    "PendingPassword",
    "Disconnected"];

function onStateUpdated(state) {
    console.log("Telegram State Changed " + telegramSessionState[state]);

    var status = document.getElementById("telegramStatus");
    status.innerText = telegramSessionState[state];
    if (state === 4) {
        status.className = "alert-info";
    } else {
        status.className = "alert-danger";
    }
    var oldStatus = document.getElementById("loadState");
    if (oldStatus != undefined) {
        if (oldStatus.value !== status.innerText) {
            window.location.reload();
        }
    }
};

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/admin-hub")
        .configureLogging(signalR.LogLevel.Information)
        .withAutomaticReconnect()
        //.withCredentials()
        .build();

    //connection.onReconnected(connectionId => {
    //    if (connection.state === signalR.HubConnectionState.Connected)
    //    {
    //        sigEvents.OnConnected(connection);
    //    };
    //
    //    console.log(`Connection reestablished. Connected with connectionId "${connectionId}".`);
    //});

    connection.on("OnStateUpdated", (e) => onStateUpdated(e));

    connection.on("OnNewMessage", (e) => sigEvents.OnNewMessage(e));
    
    connection.start()
        .then(() => sigEvents.OnConnected())
        .catch(err => sigEvents.OnError(err));
};

setupConnection();
