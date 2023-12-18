import { User } from '../models';
import { IApiResult, apiPromise } from './apiServicePromise.ts';

interface AuthenticateResult {
    token: string;
    user: User;
}

export interface IAccountApi {
    register(
        username: string,
        password: string,
        confirmPassword: string
    ): Promise<IApiResult<User>>;
    login(
        username: string,
        password: string
    ): Promise<IApiResult<AuthenticateResult>>;
}


export const accountApiService = (): IAccountApi => {
    //const baseUrl = process.env.REACT_APP_API_URL;

    const url = (url: string) => `https://localhost:7043/account/${url}`;
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

            //const data = await response.json();
            return apiPromise('success');
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
            console.log('register',response)
            if (response.status !== 201) return apiPromise('error');
            const data = await response.json();
            return apiPromise('success', data);
        }
    };
};
