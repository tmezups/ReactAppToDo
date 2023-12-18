import { useEffect, useState } from 'react';
import {IContent, ICurrentUser} from "./UserContext.ts";
import {accountApiService} from "./services";

export const useAuthenticatedApi = () : IContent => {
    const [user, setUser] = useState<ICurrentUser>({username: ''});
    const [loading, setLoading] = useState(true);
    useEffect(() => {
        const checkUser = async () => {
            let loggedInUser: ICurrentUser | null = null;
            if(localStorage.getItem('user')) {
                loggedInUser = JSON.parse(localStorage.getItem('user') || '{}');
            }
            const {user} = accountApiService();
            if (loggedInUser === null) {
                const userData = await user();
                if (userData.status === 'success' && userData.data !== null) {
                    sessionStorage.setItem('user', userData.data!.userName);
                    setUser({ username: userData.data!.userName });
                }

                setLoading(false);
            }
        };
        checkUser();
    }, []);
    
    return { user, loading }
}
