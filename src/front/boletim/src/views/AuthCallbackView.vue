<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { api } from '@/services/api'
import type { TokenResponse } from '@/types/tokenResponse'

const route = useRoute()
const router = useRouter()

onMounted(async () => {
  const code = route.query.code
  const verifier = sessionStorage.getItem('code_verifier')

  if (code && verifier) {
    try {
      // Entrega o código do Google e o seu verificador para o .NET processar
      const tokens: TokenResponse = await api.post<TokenResponse>('/auth/google-login', {
        code,
        codeVerifier: verifier,
        redirectUri: 'http://localhost:5173/auth/callback',
      })

      // Salva os tokens locais da sua API
      localStorage.setItem('access_token', tokens.accessToken)
      localStorage.setItem('refresh_token', tokens.refreshToken)

      sessionStorage.removeItem('code_verifier')
      router.push('/dashboard')
    } catch (error) {
      console.error('Falha na autenticação', error)
    }
  }
})
</script>
