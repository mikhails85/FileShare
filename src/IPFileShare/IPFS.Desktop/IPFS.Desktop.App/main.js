const electron = require('electron')
const { ipcMain } = require('electron')

// Module to control application life.
const app = electron.app
    // Module to create native browser window.
const BrowserWindow = electron.BrowserWindow

const path = require('path')
const url = require('url')
const process = require('child_process').spawn;
const portfinder = require('detect-port');
const fs = require('fs');

let io
let apiProcess
    // Keep a global reference of the window object, if you don't, the window will
    // be closed automatically when the JavaScript object is garbage collected.
let mainWindow

function createWindow() {
    // Create the browser window.
    mainWindow = new BrowserWindow({ width: 800, height: 600 })

    // and load the index.html of the app.
    mainWindow.loadURL(url.format({
        pathname: path.join(__dirname, 'ui', 'index.html'),
        protocol: 'file:',
        slashes: true
    }))

    // Open the DevTools.
    //mainWindow.webContents.openDevTools()

    // Emitted when the window is closed.
    mainWindow.on('closed', function() {
        // Dereference the window object, usually you would store windows
        // in an array if your app supports multi windows, this is the time
        // when you should delete the corresponding element.
        mainWindow = null
    })
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.on('ready', startSocketApiBridge)

// Quit when all windows are closed.
app.on('window-all-closed', function() {
    // On OS X it is common for applications and their menu bar
    // to stay active until the user quits explicitly with Cmd + Q
    if (process.platform !== 'darwin') {
        app.quit()
    }
})

app.on('activate', function() {
    // On OS X it's common to re-create a window in the app when the
    // dock icon is clicked and there are no other windows open.
    if (mainWindow === null) {
        createWindow()
    }
})

// In this file you can include the rest of your app's specific main process
// code. You can also put them in separate files and require them here.
function startSocketApiBridge() {
    portfinder(8000, (error, port) => {

        io = require('socket.io')(port);
        startBridgeBackend(port);

        io.on('connection', (socket) => {
            console.log('ASP.NET Core Application connected...');
            createWindow();

            ipcMain.on('UI:SeyHello', (event, arg) => {
                io.emit('Api:SeyHello');
                io.on('App:SeyHello', function(data) {
                    event.sender.send('App:SeyHello', data);
                });
            })

        });
    });
}

function startBridgeBackend(electronPort) {

    const parameters = ['/electronPort=' + electronPort];

    const binFilePath = path.join(__dirname, 'api', 'IPFS.Desktop.Bridge.exe');

    apiProcess = process(binFilePath, parameters);

    apiProcess.stdout.on('data', (data) => {
        var text = data.toString();
        console.log(`stdout: ${data.toString()}`);
    });

}