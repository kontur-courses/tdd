import FirebaseClient from "./firebaseClient";

let firebaseClient;
export default class ResultReporter {
    constructor() {
        firebaseClient = new FirebaseClient();
    }

    async reportResults(data, names) {
        let path = "bowling/"+(new Date()).toLocaleDateString("ru-RU") + "/" + names + "/tests";
        await firebaseClient.writeAsync(data, path);
        firebaseClient.close();
    }
}