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
        const requestId = crypto.randomUUID();
        return new Promise((resolve) => {
            this.listeners.set(requestId, (response) => resolve(response));
            this.socket?.send(JSON.stringify({ requestId, type: "AAS_READ", path }));
        });
    }
}
