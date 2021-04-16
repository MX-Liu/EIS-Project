const SerialPort = require('serialport')
const Readline = require('@serialport/parser-readline')
var data_buff;
var express = require('express');
var path = require("path");
var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var port = process.env.PORT || 9500;
var dataFIFO = [];

function readdata(data)
{
	data_buff = data;
	dataFIFO.push(data);
	//console.log('receive data :' + data);
}

const serial_port = new SerialPort('COM8', {
        baudRate: 230400,
        dataBits: 8,
        parity: 'none',
        stopBits: 1
    });


const parser = new Readline()
serial_port.pipe(parser)

parser.on('data', line => readdata(`${line}`))


app.use('/',express.static(path.join(__dirname,'/static')));

app.get('/', function(req, res){
  res.sendFile(__dirname + '/index1.html');
});


io.on('connection', function(socket){
  socket.on('chat message', function(msg){
	console.log('receive data :' + msg);
	
	//console.log('receive data :' + dataFIFO.shift());
    io.emit('chat message', dataFIFO.shift());
  }); 
});

http.listen(port, function(){
  console.log('listening on *:' + port);
});







