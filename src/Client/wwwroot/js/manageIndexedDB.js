window.manageIndexedDB = { 
    borrarDB: () => {
        let _reqDelete = window.indexedDB.deleteDatabase('clientes');
        _reqDelete.addEventListener('success', () => console.log('DB borrada con exito'));
        _reqDelete.addEventListener('error', () => console.log('error al borrar DB...', err));
    },
    almacenaClienteJWT: (datosCliente, token) => {
        try {
            let _reqDb = window.indexedDB.open('clientes');
            _reqDb.addEventListener('upgradeneeded', (ev) => {
                let _db     = _reqDb.result;
                let _store  = _db.createObjectStore('infoClientes', { autoIncrement: true });
                let _store2 = _db.createObjectStore('tokens', { autoIncrement: true });
                let _store3 = _db.createObjectStore('itemsPedidoActual', { autoIncrement: true });
            });

            _reqDb.addEventListener('success', (ev) => {
                let _db      = _reqDb.result;
                let _transac = _db.transaction(['infoClientes', 'tokens'], "readwrite");

                let _insertReq = _transac.objectStore('infoClientes').add(datosCliente);
                _insertReq.addEventListener('success', (ev) => console.log('datos cliente almacenados OK'));
                _insertReq.addEventListener('error', (err) => { throw err; });

                let _insertJWT = _transac.objectStore('tokens').add(token);
                _insertJWT.addEventListener('success', (ev) => console.log('datos JWT almacenados OK'));
                _insertJWT.addEventListener('error', (err) => { throw err });
            });
            _reqDb.addEventListener('error', (err) => { throw err });
        } catch (error) {
            console.log('error a la hora de almacenar datos cliente en indexedDB', error);
        }
    },
    checkIsCustomerLogged: () => {
        let _reqDb = window.indexedDB.open('clientes');

        _reqDb.addEventListener('success', (ev) => {
            let _db = _reqDb.result;

            if (_db.objectStoreNames.contains('infoClientes')) {
                let _selectReq = _db.transaction(['infoClientes'], 'readonly').objectStore('infoClientes').getAll();

                _selectReq.addEventListener('success', (evt) => {
                    let _datos = _select.result;
                    if (_datos.lenght > 0) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });
                _selectReq.addEventListener('error', (err) => {
                    return false;
                });
            }
            else {
                return false; 
            }
        });
        _reqDb.addEventListener('error', (err) => {
            return false;
        });
    },
    almacenarItemsPedido: (items) => { 
        let _reqDb = window.indexedDB.open('clientes');

        _reqDb.addEventListener('success', (ev) => {
            let _db        = _reqDb.result;
            let _transac   = _db.transaction(['itemsPedidoActual'], 'readwrite');
            let _insertReq = _transac.objectStore('itemsPedidoActual').add(items);

            _insertReq.addEventListener('success', (ev) => console.log('items pedido actual almacenados ok'))
            _insertReq.addEventListener('error', (err) => { console.log('error al almacenar items pedido'); throw err; })
        });
    },
    devuelveCliente: (dotnetReferenceSrv) => {
        var _promesaCliente = new Promise((resolve, reject) => {

            let _reqDb = window.indexedDB.open('clientes');

            _reqDb.addEventListener('success', () => {
                let _db = _reqDb.result;
                let _transac = _db.transaction(['infoClientes'], "readonly");
                let _selectReq = _transac.objectStore('infoClientes').getAll();

                _selectReq.addEventListener('success', () => {
                    let _datos = _selectReq.result;
                    resolve(_datos[0]);
                });
                _selectReq.addEventListener('error', (err) => { reject(err) });
            });
            _reqDb.addEventListener('error', (err) => reject(err));
        });

        _promesaCliente.then(datosCliente => { dotnetReferenceSrv.invokeMethodAsync('BlazorDBCallback', datosCliente); })
            .catch(errores => console.log('errores en operacion recp.Cliente indexedDB..', errores));
    },
    devuelveItemsPedido: (dotnetReferenceSrv) => {
        var _promisePedido = new Promise((resolve, reject) => {
                let _reqDb = window.indexedDB.open('clientes');

                _reqDb.addEventListener('success', (ev) => {
                    let _db        = _reqDb.result;
                    let _selectReq = _db.transaction(['itemsPedidoActual'], 'readonly').objectStore('itemsPedidoActual').getAll();

                    _selectReq.addEventListener('success', (evt) => {
                        let _arrdatos = _selectReq.result;

                        if (_arrdatos[0] != undefined && _arrdatos[0] != null) {
                            resolve(_arrdatos[0]);
                        }
                        else {
                            resolve(null)
                        }
                    });
                    _selectReq.addEventListener('error', (err) => {
                        resolve(null);
                    });
                });
                _reqDb.addEventListener('error', (err) => {
                    resolve(null);
                });
        });

        _promisePedido.then(arrdatos => { dotnetReferenceSrv.invokeMethodAsync('BlazorDBCallbackItemsPedido', arrdatos); })
                      .catch((error) => {console.log('error al devolver itemspedido')})
    }
}