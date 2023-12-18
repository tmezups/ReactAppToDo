import { TodoItem } from '../models';
import { IApiResult, apiPromise } from './apiServicePromise.ts';
import { NavigateFunction  } from 'react-router-dom';

export interface ITodoApi {
    createTodo(todo: TodoItem): Promise<IApiResult<TodoItem>>;
    deleteTodo(id: string): Promise<IApiResult<void>>;
    updateTodo(todo: TodoItem): Promise<IApiResult<void>>;
    getTodos(): Promise<IApiResult<TodoItem[]>>;
}

export const todoApiService = (history: NavigateFunction): ITodoApi => {
//    const baseUrl = process.env.REACT_APP_API_URL;

    const url = (url: string) => `https://localhost:7043/todo/${url}`;

    const headers = () => {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        return headers;
    };

    return {
        async createTodo(todo: TodoItem) {
            const response = await fetch(url('create'), {
                method: 'POST',
                headers: headers(),
                body: JSON.stringify(todo),
                credentials: 'include',
            });
            if (response.status === 401) { history("\login") }
            if (response.status !== 201) return apiPromise('error');
            const data = await response.json();
            return apiPromise('success', data);
        },  
        async deleteTodo(id: string) {
            const response = await fetch(url(`delete/${id}`), {
                method: 'DELETE',
                headers: headers(),
                credentials: 'include',
            });
            if (response.status === 401) { history("\login") }
            if (response.status >= 400) return apiPromise('error');
            return apiPromise('success');
        },
        async getTodos() {
            const response = await fetch(url(`getall`), {
                method: 'GET',
                headers: headers(),
                credentials: 'include',
            });
            if (response.status === 401) { history("\login") }
            if (response.status !== 200) return apiPromise('error');
            const data = await response.json();
            return apiPromise('success', data);
        },
        async updateTodo(todo: TodoItem) {
            const response = await fetch(url(`update`), {
                method: 'PUT',
                headers: headers(),
                body: JSON.stringify(todo),
                credentials: 'include',
            });
            if (response.status === 401) { history("\login") }
            if (response.status >= 400) return apiPromise('error');
            return apiPromise('success');
        },
    };
};
