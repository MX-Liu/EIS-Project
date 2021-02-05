var SerialPort = require("serialport");
var data_buffer;

serialPort = new SerialPort('COM8', {
        baudRate: 230400,
        dataBits: 8,
        parity: 'none',
        stopBits: 1
    });
	


serialPort.on("open", function () {
        console.log('open');
        serialPort.on('data', function (data) {
            console.log('data received: ' + data);
            
        });
});
	

