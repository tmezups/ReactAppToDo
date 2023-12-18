import { createContext } from 'react';

export interface ICurrentUser {
    username: string
}
export interface IContent {
    user: ICurrentUser;
    loading: boolean;
}

export const UserContext = createContext<IContent>({
    user: { username: '' },
    loading: true
});
