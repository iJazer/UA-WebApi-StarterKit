export class AASWebSocketClient {
    private socket: WebSocket | null = null;
    private listeners = new Map<string, (data: any) => void>();

    constructor(private wsUrl: string) { }

    connect() {
        if (!this.socket || this.socket.readyState !== WebSocket.OPEN) {
            this.socket = new WebSocket(this.wsUrl);
            this.socket.onmessage = (event) => {
                const data = JSON.parse(event.data);
                const callback = this.listeners.get(data.requestId);
                if (callback) {
                    callback(data);
                    this.listeners.delete(data.requestId);
                }
            };
        }
    }

    isConnected(): boolean {
        return this.socket?.readyState === WebSocket.OPEN;
    }

    async send(path: string): Promise<any> {
        const requestId = AASWebSocketClient.generateUUIDv4();
        return new Promise((resolve) => {
            this.listeners.set(requestId, (response) => resolve(response));
            this.socket?.send(JSON.stringify({ requestId, type: "AAS_READ", path }));
        });
    }

    static generateUUIDv4(): string {
    // Generates a random UUID v4 string
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        const r = Math.random() * 16 | 0;
        const v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
    }
}
