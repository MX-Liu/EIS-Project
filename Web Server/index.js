var express = require('express');
var path = require("path");
var serialScan = require('./serialdriver/serialscan');
var serialData = require('./serialdriver/serialdata');

var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var port = process.env.PORT || 8000;


serialData.open('COM8',230400,8,'none',1);

app.use('/',express.static(path.join(__dirname,'/static')));

app.get('/', function(req, res){
  res.sendFile(__dirname + '/index1.html');
});


io.on('connection', function(socket){
	
  socket.on('chat message', function(msg){
	dataBuffer = serialData.read();
    io.emit('chat message', dataBuffer);
  });
  
  // 使用计时器向客户端发送数据
	/* setInterval(() => {
	dataBuffer = serialData.read();
	io.emit('chat message', dataBuffer) 
	
  }, 100); */
  
});


http.listen(port, function(){
  console.log('listening on *:' + port);
});


