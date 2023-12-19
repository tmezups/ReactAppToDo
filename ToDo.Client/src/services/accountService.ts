import { User } from '../models';
import { IApiResult, apiPromise } from './apiServicePromise.ts';
import { NavigateFunction  } from 'react-router-dom';



export interface IAccountApi {
    register(
        username: string,
        password: string,
        confirmPassword: string
    ): Promise<IApiResult<User>>;
    login(
        username: string,
        password: string
    ): Promise<IApiResult<User>>;
    logout(history: NavigateFunction) : void;
    user(): Promise<IApiResult<User>>;
}


export const accountApiService = (): IAccountApi => {
    const url = (url: string) => `http://localhost:8080/account/${url}`;
    const headers = new Headers()
    headers.append('Content-Type', 'application/json');
    
    return {
        async login(username: string, password: string) {
            const bodyData = {
                username,
                password,
            };
            const response = await fetch(url('login'), { 
                headers: headers,
                method: 'POST',
                body: JSON.stringify(bodyData),
                credentials: 'include',
            });

            if (response.status === 404) return apiPromise('not found');
            if (response.status !== 200) return apiPromise('error');

            const data = await response.json();
            return apiPromise('success', data);
        },
      
        async register(
            username: string,
            password: string,
            confirmPassword: string
        ) {
            const bodyData = {
                username,
                password,
                confirmPassword,
            };
            const response = await fetch(url('register'), {
                method: 'POST',
                headers: headers,
                body: JSON.stringify(bodyData),
                credentials: 'include',
            });
            if (response.status !== 201) return apiPromise('error');
            const data = await response.json();
            return apiPromise('success', data);
        },
        
        async user() {
            const response = await fetch(url('user'), {
                method: 'GET',
                headers: headers,
                credentials: 'include',
            });
            if (response.status !== 200) return apiPromise('error');
            const data = await response.json();
            return apiPromise('success', data);
        },

        async logout(history: NavigateFunction) {
            await fetch(url('logout'), {
                method: 'GET',
                headers: headers,
                credentials: 'include',
            });
            history("/Login")
        }
    };
};
