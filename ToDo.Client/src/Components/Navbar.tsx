import React, {useContext} from "react";
import {
    AppBar,
    Toolbar,
    CssBaseline,
    Typography,
} from "@mui/material";
import {Link} from "react-router-dom";
import {makeStyles} from 'tss-react/mui'
import {UserContext} from "./UserContext.tsx";
import { useNavigate  } from 'react-router-dom';
import {accountApiService} from "../services";


const useStyles = makeStyles()({
    navlinks: {
        marginLeft: "auto",
        display: "flex"
    },
    logo: {
        flexGrow: "1",
        cursor: "pointer",
    },
    link: {
        textDecoration: "none",
        color: "white",
        fontSize: "20px",
        marginLeft: 20,
        "&:hover": {
            color: "yellow",
            borderBottom: "1px solid white",
        },
    }
    });

export const NavBar: React.FC = () => {
    const {classes} = useStyles();
    const value = useContext(UserContext);
    const history = useNavigate();
    const apiService = accountApiService();

    const handleLogout = async () => {
        value.logout()
        await apiService.logout(history);
    };

    return (
        <AppBar position="static">
            <CssBaseline/>
            <Toolbar>
                <Typography variant="h4">
                    To Do
                </Typography>
                <div className={classes.navlinks}>
                    <Link to="/" className={classes.link}>
                        Home
                    </Link>
                    {!value.user ?
                        <>
                            <Link to="/Login" className={classes.link}>
                                Login
                            </Link>
                            <Link to="/Register" className={classes.link}>
                                Register
                            </Link>
                        </> :
                        <Link to="" onClick={handleLogout} className={classes.link}>
                            Logout
                        </Link>
                    }

                </div>
            </Toolbar>
        </AppBar>
    );
}