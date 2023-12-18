import React, { useState } from 'react';
import {
    TextField,
    Button,
    Typography,
    Container,
    Box,
    Grid,
    Link,
    FormLabel
} from "@mui/material";
import { useNavigate  } from 'react-router-dom';
import {accountApiService} from "../../services";
export const Login: React.FC = () => {
    const history = useNavigate();
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errors, setErrors] = useState({
        username: '',
        password: '',
        apiResult:''
    });

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if(!validateForm()) return;
        const { login } = accountApiService();
        const response = await login(username, password);
        console.log('login',response);
        if (response.status === 'success') { // && response.data
            //const { token } = response.data;
           // window.sessionStorage.setItem('token', token);
            history('/');
        }
        setErrors({username: '', password: '', apiResult:'Invalid username or password'});
    };


    const validateForm = () => {
        let valid = true;
        const newErrors = { username: '', password: '' , apiResult:''};

        if (!username) {
            newErrors.username = 'Username is required';
            valid = false;
        }

        // Password strength check
        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z]).{6,}$/;
        if (!password || !passwordRegex.test(password)) {
            newErrors.password = 'Password must be at least 6 characters with at least one uppercase and one lowercase letter';
            valid = false;
        }

        setErrors(newErrors);
        return valid;
    };

    return (
        <Container component="main" maxWidth="xs">
            <Box
                sx={{
                    marginTop: 8,
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "center",
                }}
            >
                <Typography component="h1" variant="h5">
                    Sign in
                </Typography>
                <Box component="form" onSubmit={handleSubmit} noValidate sx={{mt: 1}}>
                    <TextField margin="normal"
                               required
                               fullWidth
                               id="userName"
                               label="username"
                               name="username"
                               autoComplete="username"
                               error={Boolean(errors.username)}
                               helperText={errors.username}
                               autoFocus
                               onChange={e => setUsername(e.target.value)}
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="password"
                        label="Password"
                        type="password"
                        id="password"
                        error={Boolean(errors.password)}
                        helperText={errors.password}
                        autoComplete="current-password"
                        onChange={e => setPassword(e.target.value)}
                    />
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{mt: 3, mb: 2}}
                    >
                        Login
                    </Button>
                    <Grid container>
                        <Grid item>
                               <FormLabel error={Boolean(errors.apiResult)} hidden={!Boolean(errors.apiResult)}>Please check your details and try again</FormLabel>
                        </Grid>
                        <Grid item>
                            <Link href="Register" variant="body2">
                                {"Don't have an account? Sign Up"}
                            </Link>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
        </Container>
    );
}