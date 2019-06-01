#-------------------------------------------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See https://go.microsoft.com/fwlink/?linkid=2090316 for license information.
#-------------------------------------------------------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/core/sdk:latest

# Install git, process tools
RUN apt-get update && apt-get -y install git procps

# Install Node.js and git2semver
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN apt-get install nodejs
RUN npm install -g git2semver

# Clean up
RUN apt-get autoremove -y \
    && apt-get clean -y \
    && rm -rf /var/lib/apt/lists/*

# Set the default shell to bash instead of sh
ENV SHELL /bin/bash