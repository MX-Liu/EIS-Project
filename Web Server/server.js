const
    io = require("socket.io"),
    server = io.listen(9500);

let
    sequenceNumberByClient = new Map();


// event fired every time a new client connects:
server.on("connection", (socket) => {
    console.log('Client connected [id=${socket.id}]',socket.id);
    sequenceNumberByClient.set(socket, 1);

    // when socket disconnects, remove it from the list:
    socket.on("disconnect", () => {
        sequenceNumberByClient.delete(socket);
        console.log('Client gone [id=${socket.id}]');
    });
});

// sends each client its current sequence number
setInterval(() => {
	
    for (const [client, sequenceNumber] of sequenceNumberByClient.entries()) {
        client.emit("result", "this message from server");
        sequenceNumberByClient.set(client, sequenceNumber + 1);
    }
}, 1000);