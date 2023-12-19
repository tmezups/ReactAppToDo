import { afterAll, afterEach, beforeAll } from 'vitest'
import { setupServer } from 'msw/node'
import { HttpResponse, http } from 'msw'
import { fetch } from 'cross-fetch';

global.fetch = fetch;


export const restHandlers = [
    http.get('http://localhost:8080/todo/getall', () => {
        return HttpResponse.json([])
    }),
    http.post('http://localhost:8080/todo/create', () => {
        return HttpResponse.json({toDoId: "34", title: "first post title", isDone: false}, { status: 201 })
    }),
]


const server = setupServer(...restHandlers)

// Start server before all tests
beforeAll(() => server.listen({ onUnhandledRequest: 'error' }))

//  Close server after all tests
afterAll(() => server.close())

// Reset handlers after each test `important for test isolation`
afterEach(() => server.resetHandlers())