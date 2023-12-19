import React, { useState } from 'react';
import {
    TextField,
    Button,
    Typography,
    Container,
    Box
} from "@mui/material";
import { useNavigate  } from 'react-router-dom';
import {accountApiService} from "../../services";
export const Register: React.FC = () => {
    const history = useNavigate();
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [errors, setErrors] = useState({
        username: '',
        password: '',
        confirmPassword: '',
        apiResult:''
    });

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if(!validateForm()) return;
        const { register } = accountApiService();
        const response = await register(username, password, confirmPassword);
        if (response.status === 'success' && response.data) {
            history('/Login');
        }
        setErrors({username: '', password: '', confirmPassword:'', apiResult: `{response.status}` });
    };


    const validateForm = () => {
        let valid = true;
        const newErrors = { username: '', password: '', confirmPassword: '' , apiResult:''};

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
        
        if (!confirmPassword) {
            newErrors.confirmPassword = 'Confirm password is required';
            valid = false;
        }


        if (password !== confirmPassword) {
            newErrors.confirmPassword = 'Confirm password must be the same as password';
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
                    Register
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

                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="confirmpassword"
                        label="Confirm Password"
                        type="password"
                        id="confirmpassword"
                        error={Boolean(errors.confirmPassword)}
                        helperText={errors.confirmPassword}
                        autoComplete="current-password"
                        onChange={e => setConfirmPassword(e.target.value)}
                    />
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{mt: 3, mb: 2}}
                    >
                        Register
                    </Button>
                </Box>
            </Box>
        </Container>
    );
}