# RAG Overview

## Flow

1. User enters a question
   ↓
2. Convert question to embedding (Azure OpenAI)
   ↓
3. Vector similarity search in PostgreSQL (pgvector)
   ↓
4. Retrieve top-K relevant products/reviews
   ↓
5. Build context prompt with retrieved data
   ↓
6. Send to LLM (Azure OpenAI GPT-4o-mini for cheap inference)
   ↓
7. Return answer to user

## References

PostgreSQL vector extension in .NET, see the Entity Framework Core section: https://github.com/pgvector/pgvector-dotnet

.NET AI implementations: https://github.com/azure-Samples/eshoplite

## Azure OpenAI manual configuration

- Azure Portal > Create a resource > Azure OpenAI > Create (ex.: ai-openai-labs999)
- an MS Foundry resource is created with the Azure OpenAI, look for it at https://ai.azure.com/
- get endpoint in MS Foundry: https://ai-openai-labs999.openai.azure.com/
- make model deployments (see Infrastructure-as-code provisioning at `infra/` for details)
