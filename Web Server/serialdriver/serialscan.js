//var serialPort = require("serialport");
const SerialPort = require('serialport');
var serialArray = [];

function scanCom(){
    if(serialArray.length){
        serialArray = [];
    }
	SerialPort.list().then(ports => {
		ports.forEach(function(port) {
			//console.log(port.comName);
			var thePort = {
                'comName':port.comName,
                'pnpId':port.pnpId,
                'manufacturer':port.manufacturer
            };
            if(port.comName){
                serialArray.push(thePort);
            }
			
		});
	});

    /* serialPort.list(function (err, ports) {
        ports.forEach(function(port) {
            var thePort = {
                'comName':port.comName,
                'pnpId':port.pnpId,
                'manufacturer':port.manufacturer
            };
            if(port.comName){
                serialArray.push(thePort);
            }

        });

    }); */
}

scanCom();


setInterval(function () {
    scanCom();
},2000);

module.exports = function() {
    return serialArray;
}