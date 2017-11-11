import * as firebase from "firebase";

export default class FirebaseClient {
    constructor() {
        var config = {
            databaseURL: "https://fstr-home.firebaseio.com"
          };
        
        firebase.initializeApp(config);
    }

    writeAsync(data, path) {
        var database = firebase.database();
        return database.ref(path).set(
            data,
            err => { if (err) console.log("Error while submitting " + err); }
        );
    }

    close() {
        firebase.database().goOffline();
    }
}