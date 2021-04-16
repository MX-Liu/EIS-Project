var SerialPort = require("serialport");
var serialPort;
var serialData;

function serialOpen(device,baud,bits,check,stopBit) {
    
	console.log(device);
	
    serialPort = new SerialPort(device, {
        baudRate: baud,
        dataBits: bits,
        parity: check,
        stopBits: stopBit
    });
	
    serialPort.on("open", function () {
        console.log('open');
        serialPort.on('data', function (data) {
            //console.log('data received: ' + data);
            serialData += data;
        });
    });
}
function serialClose() {
    serialPort.close();
}
function serialWrite(data) {
    console.log(data);
    serialPort.write(data, function (err, results) {
        console.log('err ' + err);
        console.log('results ' + results);
    });
}
function serialRead() {
    var dataBuffer = serialData;
    serialData = '';
    return dataBuffer;
}
exports.open = serialOpen;
exports.close = serialClose;
exports.write = serialWrite;
exports.read = serialRead;