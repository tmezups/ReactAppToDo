import {useState, useEffect} from 'react';
import {createContext} from 'react';
import {accountApiService} from "./services";

export interface ICurrentUser {
    username: string
}

export interface IContent {
    user: ICurrentUser;
    loading: boolean;
}

export type UserContextType = {
    user: ICurrentUser | null,
    loading: boolean
    logout: () => void;
    setLoggedInUser: (user: ICurrentUser) => void;
};


interface Props {
    children?: React.ReactNode
}

export const UserContext = createContext<UserContextType>({
    user: null, loading: true, logout: () => { }, setLoggedInUser: () => { }
});
export const UserContextProvider: React.FC<Props> = ({children}) => {
    const [user, setUser] = useState<ICurrentUser | null>(null);
    const [loading, setLoading] = useState(true);
    useEffect(() => {
        const checkUser = async () => {
            let loggedInUser: ICurrentUser | null = null;
            if (localStorage.getItem('user')) {
                loggedInUser = JSON.parse(localStorage.getItem('user') || '{}');
            }
            const {user} = accountApiService();
            if (loggedInUser === null) {
                const userData = await user();
                if (userData.status === 'success' && userData.data !== null) {
                    sessionStorage.setItem('user', userData.data!.userName);
                    setUser({username: userData.data!.userName});
                }

                setLoading(false);
            }
        };
        checkUser();
    }, []);
    const logout = async () => {
        sessionStorage.removeItem('user');
        setUser(null)
    }

    const setLoggedInUser = (newUser: ICurrentUser) => {
        setUser(newUser)
    }

    return <UserContext.Provider value={{user, loading, logout, setLoggedInUser}}>{children}</UserContext.Provider>;
};



