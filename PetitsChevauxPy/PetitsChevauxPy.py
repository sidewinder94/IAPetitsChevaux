import asyncio
import websockets

@asyncio.coroutine
def handler(websocket: websockets, path):
    while True:
        message = yield from websocket.recv()
        if message is None:
            break

        yield from websocket.send(message)

if __name__ == '__main__':

    start_server = websockets.serve(handler, '0.0.0.0', 10000)

    asyncio.get_event_loop().run_until_complete(start_server)
    asyncio.get_event_loop().run_forever()
