<template>
  <button @click="iniciarLoginGoogle">Login with Google</button>
</template>

<script setup lang="ts">
import { generateRandomString, generateCodeChallenge } from '@/utils/pkce'

const iniciarLoginGoogle = async () => {
  const verifier = generateRandomString(64)
  sessionStorage.setItem('code_verifier', verifier) // Salva para conferir depois

  const challenge = await generateCodeChallenge(verifier)
  const clientId = '37795619433-83l7apub5jc5igc3lvroh3oeru3gmrm1.apps.googleusercontent.com'
  const redirectUri = 'http://localhost:5173/auth/callback'

  // access_type=offline e prompt=consent são OBRIGATÓRIOS para o Google liberar o Refresh Token
  const googleAuthUrl = `https://accounts.google.com/o/oauth2/v2/auth?client_id=${clientId}&redirect_uri=${redirectUri}&response_type=code&scope=openid email profile&code_challenge=${challenge}&code_challenge_method=S256&access_type=offline&prompt=consent`

  window.location.href = googleAuthUrl
}
</script>
