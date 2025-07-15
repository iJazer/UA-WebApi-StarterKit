import { AASWebSocketClient } from "./AASWebSocketClient";

const API_BASE = `https://${location.host}/api/v3.0`;
const WS_BASE = `wss://${location.host}/ws`;

let wsClient: AASWebSocketClient;

export function initAASWebSocket() {
    wsClient = new AASWebSocketClient(WS_BASE);
    wsClient.connect();
}

export async function getJson(path: string, useWs: boolean): Promise<any> {
    if (useWs && wsClient?.isConnected()) {
        return await wsClient.send(path);
    } else {
        const res = await fetch(`${API_BASE}${path}`);
        return await res.json();
    }
}

