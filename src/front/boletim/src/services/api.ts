// O Vite injeta as variáveis do .env através do import.meta.env
const BASE_URL = import.meta.env.VITE_API_BASE_URL;

async function request<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
  // Configuração padrão dos Headers
const headers = options?.headers ? new Headers(options.headers) : new Headers();



  // Se o seu .NET 10 usa [Authorize], aqui você injeta o token JWT.
  // Exemplo buscando do localStorage (ou poderia ser da sua Store do Pinia/Vue):
  const token = localStorage.getItem('access_token');

  if (!headers.has("Authorization") && token) {
    headers.set("Authorization", `Bearer ${token}`);
}

  const config: RequestInit = {
    ...options,
    headers,
  };

  try {
    const response = await fetch(`${BASE_URL}${endpoint}`, config);

    // Tratamento de erros comuns do .NET
    if (!response.ok) {
      if (response.status === 401) {
        console.error('Não autenticado. Redirecionar para login?');
        // Lógica de logout ou redirecionamento aqui
      }
      if (response.status === 403) {
        console.error('Sem permissão para acessar este recurso.');
      }

      // Captura o corpo do erro (útil se o .NET retornar um ProblemDetails)
      const errorData = await response.json().catch(() => null);
      throw new Error(errorData?.title || `Erro HTTP: ${response.status}`);
    }

    // Retornos 204 (No Content) são comuns no .NET para PUT e DELETE
    if (response.status === 204) {
      return {} as T;
    }

    return await response.json();
  } catch (error) {
    console.error('Erro na requisição da API:', error);
    throw error;
  }
}

// Exportando os métodos HTTP simplificados
export const api = {
  get: <T>(url: string) => request<T>(url, { method: 'GET' }),

  post: <T>(url: string, body: any) =>
    request<T>(url, { method: 'POST', body: JSON.stringify(body) }),

  put: <T>(url: string, body: any) =>
    request<T>(url, { method: 'PUT', body: JSON.stringify(body) }),

  delete: <T>(url: string) => request<T>(url, { method: 'DELETE' }),
};
