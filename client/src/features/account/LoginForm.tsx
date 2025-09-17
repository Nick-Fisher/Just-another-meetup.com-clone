import { useForm } from 'react-hook-form';
import { type LoginSchema, loginSchema } from '../../lib/schemas/loginSchema';
import { useAccount } from '../../lib/hooks/useAccount';
import { zodResolver } from '@hookform/resolvers/zod';
import { Box, Button, Paper, Typography } from '@mui/material';
import { LockOpen } from '@mui/icons-material';
import TextInput from '../../app/shared/components/TextInput';
import { useNavigate, useLocation, Link } from 'react-router';

export default function LoginForm() {
  const { loginUser } = useAccount();
  const navigate = useNavigate();
  const location = useLocation();
  const {
    control,
    handleSubmit,
    formState: { isValid, isSubmitting },
  } = useForm<LoginSchema>({
    mode: 'onTouched',
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginSchema) => {
    await loginUser.mutateAsync(data, {
      onSuccess: () => {
        navigate(location.state?.from || '/activities');
      },
    });
  };

  return (
    <Paper
      component={'form'}
      onSubmit={handleSubmit(onSubmit)}
      sx={{
        display: 'flex',
        flexDirection: 'column',
        gap: 3,
        p: 3,
        maxWidth: 'md',
        mx: 'auto',
        borderRadius: 3,
      }}
    >
      <Box
        display={'flex'}
        flexDirection={'column'}
        alignItems={'center'}
        justifyContent={'center'}
        gap={3}
        color={'secondary.main'}
      >
        <LockOpen fontSize='large' />
        <Typography variant={'h4'}>Sign in</Typography>
        <TextInput label='Email' name='email' control={control} />
        <TextInput
          label='Password'
          name='password'
          type='password'
          control={control}
        />
        <Button
          type='submit'
          disabled={!isValid || isSubmitting}
          variant='contained'
          size='large'
        >
          Login
        </Button>
        <Typography sx={{ textAlign: 'center' }}>
          Don't have an account?
          <Typography
            sx={{ ml: 1 }}
            component={Link}
            to='/register'
            color='primary'
          >
            Sign Up
          </Typography>
        </Typography>
      </Box>
    </Paper>
  );
}
