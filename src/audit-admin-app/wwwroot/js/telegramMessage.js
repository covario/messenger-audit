var pageChatId = parseInt(document.getElementById("chatId").value);

function onNewMessage(msg) {
    if (msg.chatId === pageChatId) {
        var table = document.getElementById("messageList");
        var row = table.insertRow(1);
        if (table.rows.length > 25) {
            table.deleteRow(table.rows.length - 1);
        }
        row.setAttribute("data-userId", "msg");
        var unixDate = new Date(Date.parse(msg.sent));
        row.insertCell(0).innerText = unixDate.toLocaleString();
        row.insertCell(1).innerText = msg.sender;
        row.insertCell(2).innerText = msg.text;
    }
};

sigEvents.OnNewMessage = (e) => onNewMessage(e);

sigEvents.OnConnected = () => {
    connection.invoke("ChatWatch", pageChatId);
};